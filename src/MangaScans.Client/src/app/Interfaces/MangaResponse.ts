
export interface Manga {
  id: string;
  name: string;
  num_chapters: number;
  categories: Category[];
  cover: Cover;
}
  
export interface Category {
  value: string;
}
  
export interface Cover {
  link: string;
}
  
export interface MangaResponse {
  pages: number;
  data: Manga[];
}
  