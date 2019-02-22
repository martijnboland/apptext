import axios from 'axios';

import { appConfig } from '../config/AppConfig';
import { getConfig } from '../api';
import { Me } from './models';

const baseUrl = `${appConfig.apiBaseUrl}/me`;

export const getMe = (): Promise<Me> => {
  return axios.get<Me>(`${baseUrl}`, getConfig())
    .then(response => response.data);
};