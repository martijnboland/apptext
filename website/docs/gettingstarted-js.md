---
id: gettingstarted-js
title: AppText for JavaScript applications
---

This page assumes that there is a working AppText installation running on port 5000 (for example [as a Docker container](installation#install-with-docker)). 

Now, we're going to use the AppText installation to localize a JavaScript application with [i18next](https://www.i18next.com/).

## i18next

The easiest way to localize a JavaScript application with AppText is by leveraging the [i18next](https://www.i18next.com/) library. It does all all the heavy lifting regarding localization, can be used in browsers or with Node and has integrations for all major frameworks.
Please check [their web site](https://www.i18next.com/) for documentation and examples.

There are many pluggable backends that provide i18next with the translations. We're using the [i18next-http-backend](https://github.com/i18next/i18next-http-backend).

## AppText as source for i18next

The i18next-http-backend can be configured to use AppText to load the resources from. It has to use the AppText translations endpoint that is specially designed for consuming AppText from 3rd party libraries. By default the output of the translations endpoint is JSON but can also output .NET .resx and GNU gettext .po formats.

For this example, we assume that there is an App in AppText created with App ID 'my_app'. This is important because the app_id is part of the endpoint url. Furthermore, we assume that AppText is available at http://localhost:5000.

The AppText translations endpoint url is then defined as: `http://localhost:5000/my_app/translations/{language}/{collection}`. A GET request will return the text fields of all Content Items that have the Translation Content Type for the given {language} and {collection}.

The i18next configuration that leverages the AppText translations endpoint can be something like:

```typescript
import i18n from 'i18next';
import Backend from 'i18next-http-backend';

const language = 'en'; // hardcoded for simplicity

i18n
  .use(Backend)
  .init({
    lng: language,
    fallbackLng: 'en',
    returnNull: false,
    backend: {
      loadPath: `https://localhost:5001/apptext/my_app/translations/{{lng}}/{{ns}}`
    },
    ns: ['labels', 'messages'],
    defaultNS: 'labels'
  });


export default i18n;
```

The AppText translations endpoint url is set as the loadPath property of the backend. i18next replaces the {{lng}} and {{ns}} variables with the actual values at runtime. 

The {{ns}} variable indicates an [i18next namespace](https://www.i18next.com/principles/namespaces). This is used to logically group localization resources and corresponds directly with an AppText collection.
In the example configuration above, two namespaces are defined: `labels` and `messages`. This means that you have to create two collections in AppText: 'labels' and 'messages', both with `Translation` as the Content Type.

## Example application

For a complete working example of a JavaScript application that uses AppText, check out the [official example app](https://github.com/martijnboland/apptext/tree/main/examples/javascriptreactexample). The i18next configuration is in i18n.ts and it also uses the i18next React integration components and hooks.

You can also expore the [source code of the AppText Admin interface](https://github.com/martijnboland/apptext/tree/main/src/AppText.AdminApp/ClientApp/src). It uses the same concepts as the example application but is a little bit more complex.