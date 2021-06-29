import {MDCTopAppBar} from '@material/top-app-bar';

export default class Main {
    topBarElement: HTMLElement
    topBar: MDCTopAppBar
    
    constructor() {
        this.topBarElement = document.querySelector(".mdc-top-app-bar")
        this.topBar = MDCTopAppBar.attachTo(this.topBarElement)
    }
}