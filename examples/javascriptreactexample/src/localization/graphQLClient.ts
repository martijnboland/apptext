import { createClient } from 'urql';

import { appTextApiBaseUrl, appTextAppId, appTextApiKey } from './config';

const client = createClient({
  url: `${appTextApiBaseUrl}/${appTextAppId}/graphql/public`,
  fetchOptions: () => {
    return {
      headers: { 'X-Api-Key': appTextApiKey },
    };
  },
});

export default client;