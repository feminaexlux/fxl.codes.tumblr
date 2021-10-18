import {MDCTopAppBar} from "@material/top-app-bar";
import {HomePage} from "./home"

declare global {
    interface Document {
        tumblrApp: Main
    }
}

class Main {
    content: HTMLElement
    home: HomePage
    pathBase: string
    topBar: MDCTopAppBar
    
    constructor() {
        this.content = document.getElementById("main-content")
        this.topBar = MDCTopAppBar.attachTo(document.querySelector(".mdc-top-app-bar"))
    }
    
    init(pathBase: string) {
        this.pathBase = pathBase
    }
    
    public doFetch<T>(controller: string, action: string, data: FormData | null = null): Promise<T[]> {
        return fetch(`${this.pathBase}/${controller}/${action}`, { method: "POST", body: data })
            .then(response => { return response.json() })
            .then(results => { return results as T[] })
    }
}

document.tumblrApp = new Main()