export interface ContentType {
  id: string,
  appId: string,
  name: string,
  description?: string,

}

export interface Field {
  name: string,
  description?: string,
  fieldType: string,
  isRequired: boolean
}