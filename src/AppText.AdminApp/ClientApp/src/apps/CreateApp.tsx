import React, { useContext } from 'react';
import { Formik, Field, FormikHelpers } from 'formik';
import { FaSave } from 'react-icons/fa';
import { toast } from 'react-toastify';

import { appConfig } from '../config/AppConfig';
import AppContext from './AppContext';
import { TextInput } from '../common/components/form';
import { useApi, useApiGet } from '../common/api';
import { App, Language } from './models';
import { CustomSelect } from '../common/components/form/CustomSelect';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

const CreateApp: React.FunctionComponent = () => {

  const { t } = useTranslation(['Labels', 'Messages']);

  const url = `${appConfig.apiBaseUrl}/apps`;
  const createApp = useApi<App>(url, 'POST');
  const languagesUrl = `${appConfig.apiBaseUrl}/languages`;
  const { data: languages } = useApiGet<Language[]>(languagesUrl);

  const { initApps, currentApp, setCurrentApp } = useContext(AppContext);

  const initialApp: App = {
    id: undefined,
    displayName: undefined,
    languages: [],
    defaultLanguage: undefined,
    isSystemApp: false
  }

  const onSubmit = (values: any, actions: FormikHelpers<any>) => {
    createApp.callApi(values)
      .then(res => {
        if (! res.ok) {
          actions.setErrors(res.errors);
        } else {
          const newApp = res.data;
          toast.success(t('Messages:AppCreated', newApp.id));
          initApps()
            .then(() => setCurrentApp(newApp));
        }
      });
  };

  const languageOptions = languages 
    ? languages.map(l => { return { value: l.code, label: `${l.code} (${l.description})` } })
    : [];

  return (
    <>
      <h1>{t('Labels:CreateApp')}</h1>
      <div className="row">
        <div className="col-lg-8">
          <Formik
            initialValues={initialApp}
            onSubmit={onSubmit}
          >
            {({ handleSubmit, values }) => {
              const defaultLanguageOptions = values.languages 
                ? languageOptions.filter(lo => values.languages.some(l => l === lo.value) )
                : [];
          
              return (
                <form onSubmit={handleSubmit}>
                  <Field name="id" label={t('Labels:AppId')} component={TextInput} />
                  <Field name="displayName" label={t('Labels:AppDisplayName')} component={TextInput} />
                  <Field name="languages" label={t('Labels:Languages')} component={CustomSelect} isMulti={true} options={languageOptions} />
                  <Field name="defaultLanguage" label={t('Labels:DefaultLanguage')} component={CustomSelect} isMulti={false} options={defaultLanguageOptions} />
                  <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />{t('Labels:SaveButton')}</button>
                  {currentApp && 
                    <Link to={{ pathname: '/' }} className="btn btn-link">{t('Labels:CancelButton')}</Link>
                  }
                </form>
              )
            }}
          </Formik>
        </div>
      </div>
    </>
  );
};

export default CreateApp;