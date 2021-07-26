export class BlogPost {
    summary: string
    slug: string
    timestamp: Date
    tumblrId: number
    parent: Blog
    content: BlogContent
}

export class Blog {
    shortUrl: string
}

export class BlogContent {
    tags: string[] = []
    content: BlogContentBlock[] = []
}

export class BlogContentBlock {
    type: string
    text: string
    subType: string
}