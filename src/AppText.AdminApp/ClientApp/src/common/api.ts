import { useState, useEffect, SetStateAction } from 'react';
import axios, { AxiosRequestConfig, AxiosResponse, AxiosError } from 'axios';
import { setIn,  } from 'formik';
import { globalValidationProperty } from '../config/constants';

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
  // when error code is 409 or 422 and response body contains errors property, return errors, otherwise rethrow
  if (err.response && (err.response.status === 409 || err.response.status === 422) && err.response.data) {
    const data = err.response.data;
    const result: IApiResult = {
      ok: false,
      errors: {}
    };
    if (data.errors && Array.isArray(data.errors)) {
      // Group errors by property name
      result.errors = data.errors.reduce((reducedValue, currentValue) => {
        const propertyName = currentValue['name'] || globalValidationProperty;
        const errorMessage = currentValue['errorMessage'] + '\n'; // TODO: params and localization
        const val = reducedValue[propertyName] || '';
        const updatedValue = val.concat(errorMessage);
        return setIn(reducedValue, propertyName, updatedValue);
      }, {});
    } else {
      result.message = String(data);
    }
    return result;
  } else {
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
    let didCancel = false;

    const getData = async () => {
      setIsError(false);
      setIsLoading(true);

      try {
        const result = await axios.get<T>(url, getConfig());
        if (! didCancel) {
          setData(result.data);
        }
      } catch (error) {
        if (! didCancel) {
          setIsError(true);
        }
      }

      setIsLoading(false);
    };

    getData();

    return () => {
      didCancel = true;
    };
  }, [url]);

  const doGet = url => {
    setUrl(url);
  };

  return { data, isLoading, isError, doGet };
}

interface ApiHookProps<T> {
  apiResult: IApiResult,
  isApiExecuting: boolean,
  callApi: (payload: T) => Promise<IApiResult>
}

export function useApi<T>(url: string, method: string): ApiHookProps<T> {
  const [isApiExecuting, setIsApiExecuting] = useState(false);
  const [apiResult, setApiResult] = useState<IApiResult>(null);

  const callApi = async (payload: T) => {
    setApiResult(null);
    setIsApiExecuting(true);

    const config = getConfig();
    config.method = method;
    config.headers['Content-Type'] = 'application/json';
    config.data = payload;

    const result = await axios(url, config)
      .then(handleApiSuccess)
      .catch(handleApiError);

    setApiResult(result);  
    setIsApiExecuting(false);

    return result;
  };

  return { apiResult, isApiExecuting, callApi };
}