import React, { useContext } from 'react';
import { RouteComponentProps } from 'react-router';
import AppContext from './AppContext';
import { Formik, Field, FormikActions } from 'formik';
import { TextInput, CustomSelect, Select } from '../common/components/form';
import { FaSave } from 'react-icons/fa';
import { appConfig } from '../config/AppConfig';
import { useApiGet, useApi } from '../common/api';
import { Language, App } from './models';
import { toast } from 'react-toastify';
import { Link } from 'react-router-dom';

const EditApp: React.FunctionComponent<RouteComponentProps> = ({ history }) => {

  const { currentApp, initApps } = useContext(AppContext);

  const url = `${appConfig.apiBaseUrl}/apps/${currentApp.id}`;
  const updateApp = useApi<App>(url, 'PUT');

  const languagesUrl = `${appConfig.apiBaseUrl}/languages`;
  const { data: languages } = useApiGet<Language[]>(languagesUrl);

  const onSubmit = (values: any, actions: FormikActions<any>) => {
    updateApp.callApi(values)
      .then(res => {
        if (res.ok) {
          toast.success(`App ${currentApp.id} updated`);
          initApps()
            .then(() => history.push('/'));
        } else { 
          actions.setErrors(res.errors);
        }
      });
  };

  const languageOptions = languages 
    ? languages.map(l => { return { value: l.code, label: `${l.code} (${l.description})` } })
    : [];

  return (
    <>
      <h2>Edit app {currentApp.id}</h2>
      <div className="row">
        <div className="col-lg-8">
          <Formik
            initialValues={currentApp}
            onSubmit={onSubmit}
            render={({ handleSubmit, values }) => {
              const defaultLanguageOptions = values.languages 
                ? languageOptions.filter(lo => values.languages.some((l: string) => l === lo.value) )
                : [];
              return (
                <form onSubmit={handleSubmit}>
                  <Field name="displayName" label="App display name" component={TextInput} />
                  <Field name="languages" label="Languages" component={CustomSelect} isMulti={true} options={languageOptions} />
                  <Field name="defaultLanguage" label="Default language" component={CustomSelect} options={defaultLanguageOptions} />
                  <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />Save</button>
                  <Link to={{ pathname: '/' }} className="btn btn-link">Cancel</Link>
                </form>
              )
            }}>
          </Formik>
        </div>
      </div>
    </>
  );
};

export default EditApp;