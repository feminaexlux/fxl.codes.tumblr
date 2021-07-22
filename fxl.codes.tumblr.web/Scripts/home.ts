import {MDCTabBar} from "@material/tab-bar"
import {MDCTextField} from "@material/textfield"

class HomePage {
    private tabBar: MDCTabBar
    private textFields: MDCTextField[] = []
    
    constructor() {
        let main = document.tumblrApp.content
        let tabBarElement = main.querySelector(".mdc-tab-bar")
        
        if (tabBarElement) this.tabBar = MDCTabBar.attachTo(tabBarElement)
        
        main.querySelector("form")
            .querySelectorAll(".mdc-text-field")
            .forEach((element: Element) => {
                this.textFields.push(MDCTextField.attachTo(element))
            })
    }
}

new HomePage()