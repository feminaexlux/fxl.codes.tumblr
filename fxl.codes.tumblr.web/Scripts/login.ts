import { MDCTextField } from "@material/textfield"

class LoginPage {
    private fields: MDCTextField[] = []
    
    constructor() {
        document.querySelectorAll("main .mdc-text-field").forEach((element: Element) => {
            this.fields.push(MDCTextField.attachTo(element))
        }) 
    }
}

new LoginPage()