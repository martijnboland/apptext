import React from 'react';
import Modal from './Modal';
import { useTranslation } from 'react-i18next';

const Confirm = ({ title, visible, onOk, onCancel, children }) => {
  const { t } = useTranslation('Labels');
  return (
    <Modal title={title} visible={visible}
      renderFooter={() => (
        <div className="modal-footer">
          <button type="button" className="btn btn-secondary" onClick={onCancel}>
            {t('Labels:CancelButton')}
        </button>
          <button type="button" className="btn btn-primary" onClick={onOk}>
            {t('Labels:OkButton')}
        </button>
        </div>
      )}>
      {children}
    </Modal>
  );
}

export default Confirm;