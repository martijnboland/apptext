import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { FaLayerGroup, FaList, FaRegEdit } from 'react-icons/fa';

import './MainMenu.scss';
import AppContext from '../apps/AppContext';

interface MainMenuProps {
}

const MainMenu: React.FunctionComponent<MainMenuProps> = (props) => {
  const { currentApp } = useContext(AppContext);

  return (
    <>
      {currentApp &&
        <ul className="main-menu">
          <li>
            <Link to="/content"><FaRegEdit /> Content</Link>
          </li>
          <li>
            <Link to="/collections"><FaList /> Collections</Link>
          </li>
          <li>
            <Link to="/contenttypes"><FaLayerGroup /> Content types</Link>
          </li>
        </ul>
      }
    </>
  );
};

export default MainMenu;