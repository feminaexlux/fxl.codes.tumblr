import {MDCLinearProgress} from "@material/linear-progress"
import {strings as ListConstants} from "@material/list/constants"
import {MDCMenu} from "@material/menu"
import {MDCTabBar} from "@material/tab-bar"
import {MDCTextField} from "@material/textfield"
import {BlogPost} from "./entities"

export class HomePage {
    private readonly blogContent: HTMLElement
    private readonly menu: MDCMenu
    
    private progress: MDCLinearProgress
    private tabBar: MDCTabBar
    private textFields: MDCTextField[] = []
    
    constructor() {
        let main = document.tumblrApp.content
        let tabBarElement = main.querySelector(".mdc-tab-bar")
        if (tabBarElement) this.tabBar = MDCTabBar.attachTo(tabBarElement)

        let menuElement = document.querySelector(".mdc-menu")
        if (menuElement) this.menu = MDCMenu.attachTo(menuElement)
        
        let progressElement = main.querySelector(".mdc-linear-progress")
        if (progressElement) {
            this.progress = MDCLinearProgress.attachTo(progressElement)
            this.progress.determinate = false    
        }
        
        this.blogContent = document.getElementById("blog-content")
        
        this.init(main)
        document.tumblrApp.home = this
    }
    
    private init(main: HTMLElement) {
        let form = main.querySelector("form")
        if (form) {
            form.querySelectorAll(".mdc-text-field")
                .forEach((element: Element) => {
                    this.textFields.push(MDCTextField.attachTo(element))
                })
        }
        
        let switcher = document.getElementById("account-changer")
        if (switcher) {
            switcher.addEventListener("click", () => {
                this.menu.open = !this.menu.open
            })    
        }

        if (this.menu) {
            this.menu.listen(ListConstants.ACTION_EVENT, (event: CustomEvent) => {
                this.menu.open = false
                let detail = event.detail as {index: number}
                let item = this.menu.items[detail.index]
                let id = item.attributes.getNamedItem("data-blog").value
                document.location.href = `/Home/Index/${id}`
            })    
        }
    }
    
    public async getPosts(id: number) {
        this.progress.open()
        
        let data = new FormData()
        data.set("blogId", id.toString())
        
        let posts = await document.tumblrApp.doFetch<BlogPost>("Home", "GetPosts", data)
        posts.forEach((post: BlogPost) => {
            let card = this.buildCard(post)
            this.blogContent.appendChild(card)
        })
        
        this.progress.close()
    }
    
    private buildCard(post: BlogPost): HTMLElement {
        console.log(post)
        let card = document.tumblrApp.buildElement("div", ["mdc-card"])
        let content = document.tumblrApp.buildElement("div", ["mdc-card__content"], {}, post.summary)
        
        card.appendChild(content)
        
        return card
    }
}

new HomePage()