export interface ContentType {
  id?: string,
  appId?: string,
  name?: string,
  description?: string,
  metaFields: Field[],
  contentFields: Field[],
  version?: number
}

export interface Field {
  name: string,
  description?: string,
  fieldType: string,
  isRequired: boolean,
  isLocalizable?: boolean
}