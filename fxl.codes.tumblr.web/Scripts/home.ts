import {MDCChipSet} from "@material/chips"
import {MDCLinearProgress} from "@material/linear-progress"
import {strings as ListConstants} from "@material/list/constants"
import {MDCMenu} from "@material/menu"
import {MDCTabBar} from "@material/tab-bar"
import {MDCTextField} from "@material/textfield"
import {BlogPost, BlogContentBlock} from "./entities"
import {ElementBuilder} from "./helpers"

export class HomePage {
    private readonly blogContent: HTMLElement
    private readonly menu: MDCMenu
    
    private chipSets: MDCChipSet[] = []
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
                debugger
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
        
        this.blogContent.querySelectorAll(".mdc-chip-set").forEach((chipSet: Element) => {
            this.chipSets.push(MDCChipSet.attachTo(chipSet))
        })
        
        this.progress.close()
    }
    
    private buildCard(post: BlogPost): HTMLElement {
        let link = `https://${post.parent.shortUrl}.tumblr.com/post/${post.tumblrId}/${post.slug}`
        let header = post.content.content.find(x => x.subType && x.subType.indexOf("heading") >= 0)?.text ?? ""
        
        let component = ElementBuilder.getInstance("div").addClass("mdc-card").setChildren(
            ElementBuilder.getInstance("a")
                .addClass("mdc-card__primary-action")
                .addAttribute("href", link)
                .addAttribute("target", "tumblr")
                .setChildren(
                    ElementBuilder.getInstance("i").addClass("material-icons", "mdc-icon-button").setText("link"),
                    ElementBuilder.getInstance("span").addClass("card-title").setText(header),
                    ElementBuilder.getInstance("span").addClass("card-date").setText(post.timestamp.toLocaleString())
                ),
            ElementBuilder.getInstance("div").addClass("mdc-card__content").setChildren(...this.buildBlocks(post.content.content)),
            this.buildTags(post.content.tags)
        )
        
        return component.build()
    }
    
    private buildBlocks(content: BlogContentBlock[]): ElementBuilder[] {
        let blocks: ElementBuilder[] = []
        
        content.forEach(block => {
            let builder: ElementBuilder
            if (!block.subType || block.subType.indexOf("heading") < 0) {
                builder = ElementBuilder.getInstance("p")
                builder.setText(block.text)
                blocks.push(builder)
            }
        })
        
        return blocks
    }
    
    private buildTags(tags: string[]): ElementBuilder {
        let chips: ElementBuilder[] = [] 
            
        tags.forEach(tag => {
            chips.push(
                ElementBuilder.getInstance("div").addClass("mdc-chip").addAttribute("role", "row").setChildren(
                    ElementBuilder.getInstance("div").addClass("mdc-chip__ripple"),
                    ElementBuilder.getInstance("i").addClass("material-icons", "mdc-chip__icon", "mdc-chip__icon--leading").setText("tag"),
                    ElementBuilder.getInstance("span").addAttribute("role", "gridcell").setChildren(
                        ElementBuilder.getInstance("span").addClass("mdc-chip__primary-action").setChildren(
                            ElementBuilder.getInstance("span").addClass("mdc-chip__text").setText(tag)
                        )
                    )
                )
            )
        })
        
        return ElementBuilder.getInstance("div").addClass("mdc-chip-set").addAttribute("role", "grid").setChildren(...chips)
    }
}

new HomePage()