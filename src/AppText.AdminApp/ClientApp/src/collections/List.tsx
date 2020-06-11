import React, { useContext } from 'react';
import { Link, RouteComponentProps } from 'react-router-dom'; 
import { FaPlus } from 'react-icons/fa';
import { Collection } from './models';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet } from '../common/api';
import { useTranslation } from 'react-i18next';

interface ListProps extends RouteComponentProps<{}> {
}

const List: React.FunctionComponent<ListProps> = ({ match }) => {
  const { t } = useTranslation(['Labels', 'Messages']);
  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/collections`;
  const { data, isLoading } = useApiGet<Collection[]>(url, []);

  return (
    <>
    {isLoading 
      ? 
        <div>{t('Messages:Loading')}</div>
      :
        <>
          <div className="d-flex flex-row justify-content-between align-items-center">
            <h2>{t('Labels:Collections')}</h2>
            <Link to={{ pathname: `${match.url}/create` }} className="btn btn-primary">
              <FaPlus className="mr-1" />
              {t('Labels:NewCollection')}
            </Link>    
          </div>
          <dl>
            {data.map(c => 
              <React.Fragment key={c.id}>
                <dt><Link to={{ pathname: `${match.url}/edit/${c.id}` }}>{c.name}</Link></dt>
                <dd>{c.contentType.name}</dd>
              </React.Fragment>
            )}
          </dl>
        </>
      }
    </>
  );
};

export default List;