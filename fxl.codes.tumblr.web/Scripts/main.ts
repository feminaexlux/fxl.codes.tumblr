import {MDCTabBar} from "@material/tab-bar"
import {MDCTopAppBar} from "@material/top-app-bar";

declare global {
    interface Document {
        tumblrApp: Main
    }
}

class Main {
    tab: MDCTabBar
    topBar: MDCTopAppBar
    
    constructor() {
        this.topBar = MDCTopAppBar.attachTo(document.querySelector(".mdc-top-app-bar"))
        this.tab = MDCTabBar.attachTo(document.querySelector(".mdc-tab-bar"))
        
        this.init();
    }
    
    private init() {
        
    }
}

document.tumblrApp = new Main();