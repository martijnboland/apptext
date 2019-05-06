import React, { useContext, useState } from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet, useApi } from '../common/api';
import { ContentType } from './models';
import { IdParams } from '../common/routeParams';
import ContentTypeForm from './ContentTypeForm';

interface EditProps extends RouteComponentProps<IdParams> {
}

const Edit: React.FunctionComponent<EditProps> = ({ match, history,  }) => {

  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/contenttypes/${match.params.id}`;
  const { data } = useApiGet<ContentType>(url);
  const updateContentType = useApi<ContentType>(url, 'PUT');
   
  const handleSave = (contentType: ContentType): Promise<any> => {
    return updateContentType.callApi(contentType)
      .then(res => {
        if (res.ok) {
          history.push('/contenttypes');
        }
        return res;
      })
  };

  return (  
    <>
      <h2>Edit content type</h2>
      {data &&
        <div className="row">
          <div className="col-lg-8">
            <ContentTypeForm contentType={data} onSave={handleSave} errors={updateContentType.apiResult && updateContentType.apiResult.errors} />
          </div>
          <div className="col-4">
          </div>        
        </div>
      }
    </>  
  );
};

export default Edit;