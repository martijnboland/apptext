import React, { useContext } from 'react';

import ContentTypeForm from './ContentTypeForm';
import { RouteComponentProps } from 'react-router';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApi } from '../common/api';
import { ContentType } from './models';
import { toast } from 'react-toastify';

const Create: React.FunctionComponent<RouteComponentProps> = ({ history }) => {

  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/contenttypes`;
  const createContentType = useApi<ContentType>(url, 'POST');

  const initialContentType: ContentType = {
    id: undefined,
    appId: currentApp.id,
    description: undefined,
    name: undefined,
    contentFields: [],
    metaFields: []
  }
   
  const handleSave = (contentType: ContentType): Promise<any> => {
    return createContentType.callApi(contentType)
      .then(res => {
        if (res.ok) {
          toast.success(`Content type ${contentType.name} created`);
          history.push('/contenttypes');
        }
        return res;
      })
  };

  return (
    <>
      <h2>Create content type</h2>
      <div className="row">
        <div className="col-lg-8">
          <ContentTypeForm contentType={initialContentType} onSave={handleSave} />
        </div>
        <div className="col-4">
        </div>        
      </div>
    </>
  );
};

export default Create;