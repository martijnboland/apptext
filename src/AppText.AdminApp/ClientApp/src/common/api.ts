import { useState, useEffect } from 'react';
import axios, { AxiosRequestConfig, AxiosResponse, AxiosError } from 'axios';

export interface IApiResult {
  ok: boolean,
  message?: string,
  errors?: any,
  data?: any
}

export interface IPagedApiResult<T> {
  data: T[],
  totalCount: number,
  pageSize: number,
  page: number
}

export const getConfig = (): AxiosRequestConfig => ({
  headers: {
//    'Authorization': authManager.getAuthorizationHeaderValue(),
    'Cache-control': 'no-cache no-store',
    'Pragma': 'no-cache'
  }
});

export const handleApiSuccess = (response: AxiosResponse): IApiResult => {
  return {
    ok: true,
    data: response.data
  }
}

export const handleApiError = (err: AxiosError): IApiResult => {
  // when error code is 400 and response body contains properties, return object with errors, else log and rethrow
  if (err.response && err.response.status === 400 && err.response.data) {
    console.log('API returned 400 error with data, returning data for error display');
    const data = err.response.data;
    const result: IApiResult = {
      ok: false,
      errors: {}
    };
    // Convert errors array to string for the result
    if (typeof data != 'string') {
      for (let prop in data) {
        if (data.hasOwnProperty(prop)) {
          if (prop === '') {
//            result.errors[FORM_ERROR] = data[prop].toString();
          } else {
            result.errors[prop] = data[prop].toString();
          }
        }
      }
    } else {
      result.message = String(data);
    }
    return result;
  } else {
    console.log(err);
    throw err;
  }
};

interface ApiGetHookProps<T> {
  data: T,
  isLoading: boolean,
  isError: boolean,
  doGet: (url: string) => void
}

export function useApiGet<T>(initialUrl: string, initialData?: T): ApiGetHookProps<T> {
  const [data, setData] = useState(initialData);
  const [url, setUrl] = useState(initialUrl);
  const [isLoading, setIsLoading] = useState(false);
  const [isError, setIsError] = useState(false);

  useEffect(() => {
    const getData = async () => {
      setIsError(false);
      setIsLoading(true);

      try {
        const result = await axios.get<T>(url, getConfig());

        setData(result.data);
      } catch (error) {
        setIsError(true);
      }

      setIsLoading(false);
    };

    getData();
  }, [url]);

  const doGet = url => {
    setUrl(url);
  };

  return { data, isLoading, isError, doGet };
}