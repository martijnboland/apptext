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
            <a href="#"><FaRegEdit /> Content</a>
          </li>
          <li>
            <a href="#"><FaList /> Collections</a>
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