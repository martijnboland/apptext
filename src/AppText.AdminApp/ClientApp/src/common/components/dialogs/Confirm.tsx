import React from 'react';
import Modal from './Modal';

const Confirm = ({ title, visible, onOk, onCancel, children }) => (
  <Modal title={title} visible={visible}
    renderFooter={() => (
      <div className="modal-footer">
        <button type="button" className="btn btn-secondary" onClick={onCancel}>
          Cancel
      </button>
        <button type="button" className="btn btn-primary" onClick={onOk}>
          OK
      </button>
      </div>
    )}>
    {children}
  </Modal>
);

export default Confirm;