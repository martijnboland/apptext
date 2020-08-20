---
id: api-public
title: Public API
---

The AppText public API is a REST (HTTP/JSON) API that allows read-only access to content for a specfic App that is stored in AppText. 

All public endpoints require an `X-API-KEY` HTTP header. See [API keys](api-keys) for more information.

## REST endpoints

:::note
We're using the default prefix `/apptext` in the documentation. If you've [configured](configuration#addapptext) AppText with a different RoutePrefix option, then replace '/apptext' with your own prefix in the endpoint url's.
:::

### Get Collections

Url: `/apptext/{appId}/collections/public?id={id}&name={name}`

Method: `GET`

Parameters:
- `appId` App ID (string, required)
- `id` Collection ID (string, optional)
- `name` Collection name (string, optional)

### Get single Collection

Url: `/apptext/{appId}/collections/public/{id}`

Method: `GET`

Parameters:
- `appId` App ID (string, required)
- `id` Collection ID (string, required)

### Get Content Items

Url: `​/apptext​/{appId}​/content​/public?id={id}&collectionid={collectionId}&contentkey={contentKey}&contentkeystartswith={contentKeyStartsWith}&first={first}&offset={offset}&orderby={orderBy}`

Method: `GET`

Parameters:
- `appId` App ID (string, required)
- `id` Content Item ID (string, optional)
- `collectionId` Collection ID (string, optional)
- `contentKey` Content Item key (string, optional)
- `contentKeyStartsWith` Content Item key starts with (string, optional)
- `first` Get the first n Content Items (integer, optional)
- `offset` Skip the first n Content Items (integer, optional)
- `orderBy` Sort Content Items by `ContentKey` or `LastModifiedAtDescending` (optional, default `ContentKey`)

### Get single Content Item

Url: `/apptext/{appId}/content/public/{id}`

Method: `GET`

Parameters:
- `appId` App ID (string, required)
- `id` Content Item ID (string, required)

### Get Translations

Url: `/apptext/{appId}/translations/public/{language}/{collection}`

Method: `GET`

Parameters:
- `appId` App ID (string, required)
- `language` Language to retrieve the translations for (string, required)
- `collection` Collection name to read the Translations from (string, optional)

:::note
(this endpoint is from the [AppText.Translations package](configuration#apptexttranslations))
:::

## GraphQL

The GraphQL endpoint allows to read AppText content in a very flexible way. The Collections are part of the GraphQL schema. Every Collection is linked to a Content Type and that defines the GraphQL schema under a Collection.

Url: `/apptext/{appId}/graphql/public`

Method: `POST`

Parameters:
- `appId` App ID (string, required)

Data:
- the GraphQL query

Example query (AppText Admin interface schema): 

```
{
  labels {
    name
    contentType {
      name
      contentFields {
        name
        fieldType
        isRequired
      }
    }
    items {
      contentKey 
      text
      lastModifiedAt
    }
  }
}
```

Result

```
{
  "data": {
    "labels": {
      "name": "Labels",
      "contentType": {
        "name": "Translation",
        "contentFields": [
          {
            "name": "text",
            "fieldType": "ShortText",
            "isRequired": true
          }
        ]
      },
      "items": [
        {
          "contentKey": "AddField",
          "text": "Add field",
          "lastModifiedAt": "2020-08-05T15:08:18.634+02:00"
        },
        {
          "contentKey": "ApiKeysFor",
          "text": "API keys for {{app}}",
          "lastModifiedAt": "2020-08-05T15:08:18.79+02:00"
        },
        {
          "contentKey": "ApiKeysHelpText",
          "text": "API keys are app-specific and can be used for read-only access to the AppText API to retrieve content.",
          "lastModifiedAt": "2020-08-05T15:08:18.805+02:00"
        },
        {
          "contentKey": "AppDisplayName",
          "text": "App display name",
          "lastModifiedAt": "2020-08-05T15:08:18.392+02:00"
        },
        ...
```

:::note
AppText contains the GraphiQL query tool that can be enabled via a [configuration option](configuration#apptextapiconfigurationoptions) `EnableGraphiql`. After enabling, GraphiQL is available at `/apptext/{appId}/graphql/graphiql`. The same authorization rules apply for GraphiQL as for the rest of the AppText API.
:::