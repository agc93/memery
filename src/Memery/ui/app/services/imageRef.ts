export class ImageRef {
    constructor(id: string, name: string, location?: string) {
        this.id = id;
        this.name = name;
        this.location = location ? location : '';
    }
    id: string;
    name: string;
    location: string;
  }

export class IndexResponse {
    [id: string]: ImageRef;
}