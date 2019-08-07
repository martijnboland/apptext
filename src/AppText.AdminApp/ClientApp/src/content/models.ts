export interface ContentItem {
  id?: string,
  appId?: string,
  contentKey?: string,
  collectionId: string,
  meta?: any
  content?: any,
  version?: number
  createdAt?: string,
  createdBy?: string,
  lastModifiedAt?: string,
  lastModifiedBy?: string
}