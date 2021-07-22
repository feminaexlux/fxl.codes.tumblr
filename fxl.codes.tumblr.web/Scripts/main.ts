import {MDCTopAppBar} from "@material/top-app-bar";

declare global {
    interface Document {
        tumblrApp: Main
    }
}

class Main {
    content: HTMLElement
    topBar: MDCTopAppBar
    
    constructor() {
        this.content = document.querySelector("#main-content")
        this.topBar = MDCTopAppBar.attachTo(document.querySelector(".mdc-top-app-bar"))
        
        this.init();
    }
    
    private init() {
        
    }
}

document.tumblrApp = new Main();