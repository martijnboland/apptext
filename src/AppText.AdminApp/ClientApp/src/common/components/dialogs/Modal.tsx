import React, { useState, useEffect } from 'react';
import classNames from 'classnames';

interface ModalProps {
  children: React.ReactNode,
  onClickBackdrop?: () => void,
  visible: boolean,
  title?: string,
  onClose?: () => void,
  renderFooter?: () => React.ReactNode
}

const Modal: React.FunctionComponent<ModalProps> = ({ children, onClickBackdrop, visible, title, onClose, renderFooter }) => {

  const [isVisible, setVisible] = useState(visible);

  const renderBackdrop = () => {
    if (isVisible) {
      return (
        <div
          className={classNames('modal-backdrop', 'fade', { show: isVisible })}
          onClick={onClickBackdrop}
          role="presentation"/>
      );
    }
    return null;
  }

  const close = () => {
    if (onClose) {
      onClose();
    } else {
      setVisible(false);
    }
  }

  return (
    <div>
      <div
        className={classNames('modal', 'fade', { show: isVisible })}
        style={{ display: (isVisible ? 'block' : 'none') }}
        role="dialog"
        aria-hidden={!isVisible}
        tabIndex={-1}
        onClick={onClickBackdrop}>
        <div className="modal-dialog" role="document">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{title}</h5>
              <button type="button" className="close" aria-label="Close" onClick={close}>
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">
              {visible && children}
            </div>
            {renderFooter && renderFooter()}
          </div>
        </div>
      </div>
      {renderBackdrop()}
    </div>
  );

}

export default Modal;