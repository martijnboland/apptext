import React, { useContext } from 'react';
import { RouteComponentProps } from 'react-router';
import { Formik, Field, FormikActions } from 'formik';
import { FaSave } from 'react-icons/fa';
import { toast } from 'react-toastify';

import { appConfig } from '../config/AppConfig';
import AppContext from './AppContext';
import { TextInput } from '../common/components/form';
import { useApi } from '../common/api';
import { App } from './models';

const CreateApp: React.FunctionComponent<RouteComponentProps> = ({ history }) => {

  const url = `${appConfig.apiBaseUrl}/apps`;
  const createApp = useApi<App>(url, 'POST');
  const { initApps, setCurrentApp } = useContext(AppContext);

  const initialApp: App = {
    id: undefined,
    displayName: undefined,
    languages: [],
    defaultLanguage: undefined
  }

  const onSubmit = (values: any, actions: FormikActions<any>) => {
    createApp.callApi(values)
      .then(res => {
        if (! res.ok) {
          actions.setErrors(res.errors);
        } else {
          const newApp = res.data;
          toast.success(`App ${newApp.id} created`);
          initApps()
            .then(() => setCurrentApp(newApp));
        }
      });
  };

  return (
    <>
      <h1>Create app</h1>
      <div className="row">
        <div className="col-lg-8">
          <Formik
            initialValues={initialApp}
            onSubmit={onSubmit}
            render={({ handleSubmit, values }) => (
              <form onSubmit={handleSubmit}>
                <Field name="id" label="App ID" component={TextInput} />
                <Field name="displayName" label="App display name" component={TextInput} />
                <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />Save</button>
              </form>
            )}>
          </Formik>
        </div>
      </div>
    </>
  );
};

export default CreateApp;