import React, { useContext } from 'react';
import { RouteComponentProps } from 'react-router';
import AppContext from './AppContext';
import { Formik, Field, FormikHelpers } from 'formik';
import { TextInput, CustomSelect, Select } from '../common/components/form';
import { FaSave } from 'react-icons/fa';
import { appConfig } from '../config/AppConfig';
import { useApiGet, useApi } from '../common/api';
import { Language, App } from './models';
import { toast } from 'react-toastify';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import ApiKeyList from './ApiKeyList';
import DeleteApp from './DeleteApp';

const EditApp: React.FunctionComponent<RouteComponentProps> = ({ history }) => {

  const { t } = useTranslation(['Labels', 'Messages']);
  const { currentApp, initApps } = useContext(AppContext);

  const url = `${appConfig.apiBaseUrl}/apps/${currentApp.id}`;
  const updateApp = useApi<App>(url, 'PUT');

  const languagesUrl = `${appConfig.apiBaseUrl}/languages`;
  const { data: languages } = useApiGet<Language[]>(languagesUrl);

  const onSubmit = (values: any, actions: FormikHelpers<any>) => {
    updateApp.callApi(values)
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:AppUpdated', { name: currentApp.id }));
          initApps()
            .then(() => history.push('/'));
        } else { 
          actions.setErrors(res.errors);
        }
      });
  };

  const handleAppDeleted = () => {
    initApps()
      .then(() => history.push('/'));
  }

  const languageOptions = languages 
    ? languages.map(l => { return { value: l.code, label: `${l.code} (${l.description})` } })
    : [];

  return (
    <React.Fragment>
      <h2>{t('Labels:EditApp',  { app: currentApp.id })}</h2>
      <div className="row mb-4">
        <div className="col-lg-8">
          <Formik
            initialValues={currentApp}
            onSubmit={onSubmit}
          >
            {({ handleSubmit, values }) => {
              const defaultLanguageOptions = values.languages 
                ? languageOptions.filter(lo => values.languages.some((l: string) => l === lo.value) )
                : [];
              return (
                <form onSubmit={handleSubmit}>
                  <Field name="displayName" label={t('Labels:AppDisplayName')} component={TextInput} />
                  <Field name="languages" label={t('Labels:Languages')} component={CustomSelect} isMulti={true} options={languageOptions} />
                  <Field name="defaultLanguage" label={t('Labels:DefaultLanguage')} component={CustomSelect} options={defaultLanguageOptions} />
                  <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />{t('Labels:SaveButton')}</button>
                  <Link to={{ pathname: '/' }} className="btn btn-link">{t('Labels:CancelButton')}</Link>
                </form>
              )
            }}
          </Formik>
        </div>
      </div>
      <div className="row">
        <div className="col-lg-8">
          <ApiKeyList />
        </div>
      </div>
      {!currentApp.isSystemApp &&
        <div className="row">
          <div className="col-lg-8">
            <DeleteApp app={currentApp} onAppDeleted={handleAppDeleted} />
          </div>
        </div>
      }
    </React.Fragment>
  );
};

export default EditApp;