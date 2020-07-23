import React from 'react';
import { useModal } from 'react-modal-hook';
import Confirm from '../common/components/dialogs/Confirm';
import { useTranslation } from 'react-i18next';
import { FaTrash } from 'react-icons/fa';
import { ApiKey } from './models';
import { useApi } from '../common/api';
import { appConfig } from '../config/AppConfig';
import { toast } from 'react-toastify';

interface IApiKeyListItemProps {
  apiKey: ApiKey,
  onApiKeyDeleted: () => void
}

const ApiKeyListItem: React.FunctionComponent<IApiKeyListItemProps> = ({ apiKey, onApiKeyDeleted }) => {
  const { t } = useTranslation(['Labels', 'Messages']);
  
  const deleteUrl = `${appConfig.apiBaseUrl}/${apiKey.appId}/apikeys/${apiKey.id}`;
  const deleteApiKey = useApi<{}>(deleteUrl, 'DELETE')
  
  const handleDelete = (apiKey: ApiKey, hideDeleteConfirmation): Promise<any> => {
    return deleteApiKey.callApi(null)
    .then(res => {
      if (res.ok) {
        toast.success(t('Messages:ApiKeyDeleted', { name: apiKey.name }));
        onApiKeyDeleted();
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

  const [showDeleteConfirmation, hideDeleteConfirmation] = useModal(() => (
    <Confirm
      visible={true}
      title={t('Labels:DeleteApiKey')}
      onOk={() => handleDelete(apiKey, hideDeleteConfirmation)}
      onCancel={hideDeleteConfirmation}
    >
      {t('Messages:DeleteConfirm', { name: '$t(Labels:TheApiKey)' })}
    </Confirm>
  ), [apiKey]);
  
  const createdAt = new Date(apiKey.createdAt).toLocaleString();

  return (
    <div className="card mb-3" key={apiKey.id}>
      <div className="card-body row">
        <h5 className="col-sm-4">
          {apiKey.name}
        </h5>
        <div className="col-sm-4">
          {createdAt}
        </div>
        <div className="col-sm-4 text-right">
          <button type="button" className="btn btn-danger" onClick={showDeleteConfirmation}>
            <FaTrash className="mr-1" />
            {t('Labels:DeleteButton')}
          </button>
        </div>
      </div>
    </div>
  )

};

export default ApiKeyListItem;
