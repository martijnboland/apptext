import React from 'react';
import { useTranslation } from 'react-i18next';
import { toast } from 'react-toastify';

interface IGeneratedApiKeyProps {
  generatedKey: string,
  onClose(): void
}

const GeneratedApiKey: React.FunctionComponent<IGeneratedApiKeyProps> = ({ generatedKey, onClose }) => {
  const { t } = useTranslation(['Labels', 'Messages']);

  const copyToClipboard = () => {
    if (navigator.clipboard && generatedKey) {
      navigator.clipboard.writeText(generatedKey)
        .then(() => toast.success(t('Messages:ApiKeyCopiedToClipboard')));
    } else {
      toast.error(t('Messages:BrowserDoesNotSupportCopyToClipboard'));
    }
  }
  return (
    <div>
      <p>{t('Labels:GeneratedApiKeyIs')}</p>
      <pre><strong>{generatedKey}</strong></pre>
      <p>
        <small className="text-muted">{t('Labels:GeneratedApiKeyHelpText')}</small>
      </p>
      <button type="button" className="btn btn-primary" onClick={copyToClipboard}>{t('Labels:CopyToClipboardButton')}</button>
      <button type="button" className="btn btn-link" onClick={onClose}>{t('Labels:CloseButton')}</button>
    </div>
  );
};

export default GeneratedApiKey;
