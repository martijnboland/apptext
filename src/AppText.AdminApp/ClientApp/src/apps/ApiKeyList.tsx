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

const ApiKeyList: React.FunctionComponent = () => {
  const { t } = useTranslation(['Labels', 'Messages']);
  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/apikeys`;
  const { data: apiKeys, isLoading, doGet: getApiKeys } = useApiGet<ApiKey[]>(url, []);
  const [isNewFormVisible, setIsNewFormVisible] = useState(false);
  const createApiKeyCommand = useApi<CreateApiKeyCommand>(url, 'POST');
  const [generatedApiKey, setGeneratedApiKey] = useState();

  const newApiKey = () => {
    setIsNewFormVisible(true);
    setGeneratedApiKey(undefined);
  }

  const closeNewApiKey = () => {
    setIsNewFormVisible(false);
  }

  const handleApiKeyDeleted = () => {
    getApiKeys(url, true);
  }

  const createApiKey = (name: string): Promise<any> => {
    return createApiKeyCommand.callApi({ appId: currentApp.id, name })
      .then(res => {
        if (res.ok) {
          setGeneratedApiKey(res.data.apiKey);
          setIsNewFormVisible(false);
          getApiKeys(url, true);
          toast.success(t('Messages:ApiKeyCreated', { name: name }));
        }
        return res;
      })
  }

  const closeGeneratedApiKey = () => {
    setGeneratedApiKey(undefined);
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
      {isNewFormVisible &&
        <div className="mb-3">
          <CreateApiKeyForm onCreate={createApiKey} onClose={closeNewApiKey} />
        </div>
      }
      {generatedApiKey &&
        <div className="mb-3">
          <GeneratedApiKey generatedKey={generatedApiKey} onClose={closeGeneratedApiKey} />
        </div>
      }
      {apiKeys && apiKeys.map(apiKey => 
        <ApiKeyListItem key={apiKey.id} apiKey={apiKey} onApiKeyDeleted={handleApiKeyDeleted} />
      )}
    </React.Fragment>
  );
};

export default ApiKeyList;
