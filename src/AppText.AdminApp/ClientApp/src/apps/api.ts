import axios from 'axios';

import { appConfig } from '../config/AppConfig';
import { getConfig } from '../common/api';
import { App } from './models';

const baseUrl = `${appConfig.apiBaseUrl}/apps`;

export const getApps = (): Promise<App[]> => {
  return axios.get<App[]>(`${baseUrl}`, getConfig())
    .then(response => response.data);
};