import {MDCTabBar} from "@material/tab-bar"
import {MDCTopAppBar} from "@material/top-app-bar";

declare global {
    interface Document {
        tumblrApp: Main
    }
}

class Main {
    tabElement: HTMLElement
    tab: MDCTabBar
    topBarElement: HTMLElement
    topBar: MDCTopAppBar
    
    constructor() {
        this.topBarElement = document.querySelector(".mdc-top-app-bar")
        this.tabElement = document.querySelector(".mdc-tab-bar")
        this.init();
    }
    
    init() {
        this.topBar = MDCTopAppBar.attachTo(this.topBarElement)
        this.tab = MDCTabBar.attachTo(this.tabElement)
    }
}

document.tumblrApp = new Main();