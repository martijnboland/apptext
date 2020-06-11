import React, { useContext } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { useModal } from 'react-modal-hook';
import { toast } from 'react-toastify';

import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet, useApi } from '../common/api';
import { ContentType } from './models';
import { IdParams } from '../common/routeParams';
import ContentTypeForm from './ContentTypeForm';
import Confirm from '../common/components/dialogs/Confirm';
import { useTranslation } from 'react-i18next';

interface EditProps extends RouteComponentProps<IdParams> {
}

const Edit: React.FunctionComponent<EditProps> = ({ match, history,  }) => {

  const { t } = useTranslation(['Labels', 'Messages']);
  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/contenttypes/${match.params.id}`;
  const { data } = useApiGet<ContentType>(url);
  const updateContentType = useApi<ContentType>(url, 'PUT');
  const deleteContentType = useApi<ContentType>(url, 'DELETE');
   
  const handleSave = (contentType: ContentType): Promise<any> => {
    return updateContentType.callApi(contentType)
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:ContentTypeUpdated', { name: data.name }));
          history.push('/contenttypes');
        }
        return res;
      });
  };

  const [showDeleteConfirmation, hideDeleteConfirmation] = useModal(() => (
      <Confirm
        visible={true}
        title={t('Labels:DeleteContentType')}
        onOk={() => handleDelete(data, hideDeleteConfirmation)}
        onCancel={hideDeleteConfirmation}
      >
        {t('Messages:DeleteConfirm', { name: 'the content type'})}
      </Confirm>
  ), [data]);

  const handleDelete = (contentType, hideDeleteConfirmation): Promise<any> => {
    return deleteContentType.callApi(contentType)
      .then(res => {
        if (res.ok) {
          toast.success(t('Messages:ContentTypeDeleted', { name: contentType.name} ));
          history.push('/contenttypes');
        }
        else {
          toast.error(Object.values(res.errors).join(','));
        }
        return res;
      })
      .catch(err => {
        hideDeleteConfirmation();
      })
  }

  return (  
    <>
      <h2>{t('Labels:EditContentType')}</h2>
      {data &&
        <div className="row">
          <div className="col-lg-8">
            <ContentTypeForm contentType={data} onSave={handleSave} onDelete={showDeleteConfirmation} />
          </div>
          <div className="col-4">
          </div>        
        </div>
      }
    </>  
  );
};

export default Edit;