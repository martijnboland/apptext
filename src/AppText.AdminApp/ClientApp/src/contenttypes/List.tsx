import React, { useContext } from 'react';
import { Link, RouteComponentProps } from 'react-router-dom'; 
import { FaPlus } from 'react-icons/fa';
import { ContentType } from './models';
import AppContext from '../apps/AppContext';
import { appConfig } from '../config/AppConfig';
import { useApiGet } from '../common/api';
import { useTranslation } from 'react-i18next';

interface ListProps extends RouteComponentProps<{}> {
}

const List: React.FunctionComponent<ListProps> = ({ match }) => {
  const { t } = useTranslation(['Labels', 'Messages']);
  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/contenttypes?includeglobalcontenttypes=true`;
  const { data, isLoading } = useApiGet<ContentType[]>(url, []);

  const ContentTypeTitle = ({ contentType }) => {
    if (contentType.appId) {
      return <Link to={{ pathname: `${match.url}/edit/${contentType.id}` }}>{contentType.name}</Link>
    } else {
      return <span>{contentType.name} (global)</span>
    }
  }

  return (
    <>
    {isLoading 
      ? 
        <div>{t('Messages:Loading')}</div>
      :
        <>
          <div className="d-flex flex-row justify-content-between align-items-center">
            <h2>{t('Labels:ContentTypes')}</h2>
            <Link to={{ pathname: `${match.url}/create` }} className="btn btn-primary">
              <FaPlus className="mr-1" />
              {t('Labels:NewContentType')}
            </Link>    
          </div>
          <dl>
            {data.map(ct => 
              <React.Fragment key={ct.id}>
                <dt><ContentTypeTitle contentType={ct} /></dt>
                <dd>{ct.description}</dd>
              </React.Fragment>
            )}
          </dl>
        </>
      }
    </>
  );
};

export default List;