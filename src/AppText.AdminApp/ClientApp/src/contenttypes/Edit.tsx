import React, { useContext, useState } from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet, useApi } from '../common/api';
import { ContentType } from './models';
import { IdParams } from '../common/routeParams';
import EditForm from './EditForm';

interface EditProps extends RouteComponentProps<IdParams> {
}

const Edit: React.FunctionComponent<EditProps> = ({ match, history,  }) => {

  const { currentApp } = useContext(AppContext);
  const [contentType, setContentType] = useState<ContentType>(null);
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
          <div className="col-lg-6">
            <EditForm contentType={data} onSave={handleSave} errors={updateContentType.apiResult && updateContentType.apiResult.errors} />
          </div>
          <div className="col-6">
            <pre>
              {contentType && JSON.stringify(contentType, null, 2)}
            </pre>
          </div>        
        </div>
      }
    </>  
  );
};

export default Edit;