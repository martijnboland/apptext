import axios from 'axios';

import { appConfig } from '../config/AppConfig';
import { getConfig } from '../common/api';
import { ContentType } from './models';

const baseUrl = `${appConfig.apiBaseUrl}`;

export const getContentTypes = (appId: string): Promise<ContentType[]> => {
  return axios.get<ContentType[]>(`${baseUrl}/${appId}/contenttypes`, getConfig())
    .then(response => response.data);
};