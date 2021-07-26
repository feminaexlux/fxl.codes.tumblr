export class ElementBuilder {
    tag: string
    classes: string[]
    attributes: {[key: string]: string}
    text: string
    children: ElementBuilder[] = []
    
    constructor(tag: string, classes: string[] = [], attributes: {[key: string]: string} = {}, text = "", children: ElementBuilder[] = []) {
        this.tag = tag
        this.classes = classes
        this.attributes = attributes
        this.text = text
        this.children = children
    }
    
    public setAttributes(attributes: {[key: string]: string}): ElementBuilder {
        this.attributes = attributes
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