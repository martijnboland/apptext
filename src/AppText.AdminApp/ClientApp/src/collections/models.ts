import { ContentType } from "../contenttypes/models";

export interface Collection {
  id?: string,
  name?: string,
  description?: string,
  contentType?: ContentType,
  listDisplayField?: string,
  version?: number
}
