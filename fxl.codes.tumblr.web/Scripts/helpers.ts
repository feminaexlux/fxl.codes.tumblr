export class ElementBuilder {
    tag: string
    classes: string[] = []
    attributes: {[key: string]: string} = {}
    text: string
    children: ElementBuilder[] = []
    
    private constructor(tag: string) {
        this.tag = tag
    }
    
    public static getInstance(tag: string): ElementBuilder {
        return new ElementBuilder(tag)
    }
    
    public addClass(...className: string[]): ElementBuilder {
        this.classes.push(...className)
        return this
    }
    
    public addAttribute(key: string, value: string): ElementBuilder {
        this.attributes[key] = value
        return this
    }
    
    public setText(text = ""): ElementBuilder {
        this.text = text
        return this
    }
    
    public setChildren(...children: ElementBuilder[]): ElementBuilder {
        this.children = children
        return this
    }
    
    public build(): HTMLElement {
        let element = document.createElement(this.tag)
        
        this.classes.forEach(cssClass => element.classList.add(cssClass))
        
        Object.keys(this.attributes).forEach(key => {
            let attribute = document.createAttribute(key)
            attribute.value = this.attributes[key]
            element.attributes.setNamedItem(attribute)
        })
        
        if (this.text) element.textContent = this.text
        
        this.children.forEach(child => {
            let childElement = child.build()
            element.appendChild(childElement)
        })
        
        return element
    }
}