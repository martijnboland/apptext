import React from 'react';
import classNames from 'classnames';
import useComponentVisible from '../common/hooks/useComponentVisible';

interface ILanguageSelectorProps {
}

const LanguageSelector: React.FunctionComponent<ILanguageSelectorProps> = (props) => {
  const { ref, isComponentVisible, setIsComponentVisible } = useComponentVisible(false);

  const toggleLanguageSelector = (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    e.preventDefault();
    setIsComponentVisible(true);
  };

  return (
    <li className="nav-item dropdown">
      <a className="nav-link dropdown-toggle" id="languageselector" href="#" aria-haspopup="true" aria-expanded="false" onClick={toggleLanguageSelector}>
        {}
      </a>
      <div ref={ref} className={classNames('dropdown-menu', 'dropdown-menu-right', { show: isComponentVisible })} aria-labelledby="languageselector">
        <a className="dropdown-item" href="#">nl</a>
        <a className="dropdown-item" href="#">de</a>
      </div>
    </li>
  );
};

export default LanguageSelector;
