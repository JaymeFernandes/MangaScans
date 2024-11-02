
export interface Manga {
  id: string;
  name: string;
  description: string;
  like: number;
  dislikes : number;
  views: number;
  num_chapters: number;
  isliked: boolean;
  isFavorite: boolean;
  categories: Category[];
  chapters?: Chapter[];
  cover: Cover;
}

export interface MangaInfoResponse{
  data: Manga;
}

export interface Chapter{
  id: number;
  num_do_Capitulo: number;
  title: string;
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
  