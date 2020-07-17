import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { FaLayerGroup, FaList, FaRegEdit } from 'react-icons/fa';

import './MainMenu.scss';
import AppContext from '../apps/AppContext';
import { useTranslation } from 'react-i18next';

interface MainMenuProps {
}

const MainMenu: React.FunctionComponent<MainMenuProps> = (props) => {
  const { t } = useTranslation('Labels');
  const { currentApp } = useContext(AppContext);

  return (
    <>
      {currentApp &&
        <ul className="main-menu">
          <li>
            <Link to="/content"><FaRegEdit /> {t('Labels:Content')}</Link>
          </li>
          <li>
            <Link to="/contenttypes"><FaLayerGroup /> {t('Labels:ContentTypes')}</Link>
          </li>
        </ul>
      }
    </>
  );
};

export default MainMenu;