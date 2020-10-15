import { createClient } from 'urql';

import { appTextApiBaseUrl, appTextAppId } from './config';

const client = createClient({
  url: `${appTextApiBaseUrl}/${appTextAppId}/graphql/public`,
  fetchOptions: () => {
    return {
      headers: { 'X-Api-Key': process.env.REACT_APP_APPTEXT_APIKEY || '' },
    };
  },
});

export default client;