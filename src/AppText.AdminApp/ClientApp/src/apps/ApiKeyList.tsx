import React, { useContext, useState } from 'react';
import { useTranslation } from 'react-i18next';
import AppContext from './AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet, useApi } from '../common/api';
import { ApiKey, CreateApiKeyCommand } from './models';
import { FaPlus } from 'react-icons/fa';
import ApiKeyListItem from './ApiKeyListItem';
import CreateApiKeyForm from './CreateApiKeyForm';
import { toast } from 'react-toastify';
import GeneratedApiKey from './GeneratedApiKey';
import { useModal } from 'react-modal-hook';
import Modal from '../common/components/dialogs/Modal';

const ApiKeyList: React.FunctionComponent = () => {
  const { t } = useTranslation(['Labels', 'Messages']);
  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/apikeys`;
  const { data: apiKeys, isLoading, doGet: getApiKeys } = useApiGet<ApiKey[]>(url, []);
  const createApiKeyCommand = useApi<CreateApiKeyCommand>(url, 'POST');
  const [generatedApiKey, setGeneratedApiKey] = useState();

  const [showNewApiKey, hideNewApiKey] = useModal(() => 
    <Modal visible={true} title={t('Labels:NewApiKey')} onClose={hideNewApiKey}>
      <CreateApiKeyForm onCreate={createApiKey} onClose={hideNewApiKey} />
    </Modal>
  );
  const [showGeneratedKey, hideGeneratedKey] = useModal(() =>
    <Modal visible={true} title="API key" onClose={closeGeneratedApiKey}>
      <GeneratedApiKey generatedKey={generatedApiKey} onClose={closeGeneratedApiKey} />
    </Modal>
  , [generatedApiKey]);

  const newApiKey = () => {
    showNewApiKey();
    setGeneratedApiKey(undefined);
  }

  const handleApiKeyDeleted = () => {
    getApiKeys(url, true);
  }

  const createApiKey = (name: string): Promise<any> => {
    return createApiKeyCommand.callApi({ appId: currentApp.id, name })
      .then(res => {
        if (res.ok) {
          setGeneratedApiKey(res.data.apiKey);
          hideNewApiKey();
          showGeneratedKey();
          getApiKeys(url, true);
          toast.success(t('Messages:ApiKeyCreated', { name: name }));
        }
        return res;
      })
  }

  const closeGeneratedApiKey = () => {
    setGeneratedApiKey(undefined);
    hideGeneratedKey();
  }

  return (
    <React.Fragment>
      <div className="d-flex flex-row justify-content-between align-items-center">
        <h2>{t('Labels:ApiKeysFor', { app: currentApp.id})}</h2>
        <button type="button" className="btn btn-primary" onClick={newApiKey}>
          <FaPlus className="mr-1" />
          {t('Labels:NewApiKey')}
        </button>    
      </div>
      <p>
        <small className="text-muted">{t('Labels:ApiKeysHelpText')}</small>
      </p>
      {apiKeys && apiKeys.map(apiKey => 
        <ApiKeyListItem key={apiKey.id} apiKey={apiKey} onApiKeyDeleted={handleApiKeyDeleted} />
      )}
    </React.Fragment>
  );
};

export default ApiKeyList;
