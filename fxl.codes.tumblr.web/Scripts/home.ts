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
        let component = new ElementBuilder("div", ["mdc-card"]).setChildren(
            new ElementBuilder("a", ["mdc-card__primary-action"], {"href": link, "target": "tumblr"}).setChildren(
                new ElementBuilder("i",
                    ["material-icons", "mdc-icon-button"],
                    {"href": link},
                    "link"),
                new ElementBuilder("span", ["card-title"]).setText(header),
                new ElementBuilder("span", ["card-date"]).setText(post.timestamp.toLocaleString())
            ),
            new ElementBuilder("div", ["mdc-card__content"]).setChildren(...this.buildBlocks(post.content.content)),
            this.buildTags(post.content.tags)
        )
        
        return component.build()
    }
    
    private buildBlocks(content: BlogContentBlock[]): ElementBuilder[] {
        let blocks: ElementBuilder[] = []
        
        content.forEach(block => {
            let builder: ElementBuilder
            if (!block.subType || block.subType.indexOf("heading") < 0) {
                builder = new ElementBuilder("p")
                builder.setText(block.text)
                blocks.push(builder)
            }
        })
        
        return blocks
    }
    
    private buildTags(tags: string[]): ElementBuilder {
        let chips: ElementBuilder[] = [] 
            
        tags.forEach(tag => {
            chips.push(new ElementBuilder("div", ["mdc-chip"], {"role": "row"}).setChildren(
                new ElementBuilder("div", ["mdc-chip__ripple"]),
                new ElementBuilder("i", ["material-icons", "mdc-chip__icon", "mdc-chip__icon--leading"]).setText("tag"),
                new ElementBuilder("span").setAttributes({"role": "gridcell"}).setChildren(
                    new ElementBuilder("span", ["mdc-chip__primary-action"]).setChildren(
                        new ElementBuilder("span", ["mdc-chip__text"]).setText(tag)
                    )
                )
            ))
        })
        
        return new ElementBuilder("div", ["mdc-chip-set"], {"role": "grid"}).setChildren(...chips)
    }
}

new HomePage()