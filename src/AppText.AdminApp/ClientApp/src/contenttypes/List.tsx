import React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom'; 
import { ContentType } from './models';

interface ListProps extends RouteComponentProps<{}> {
  contentTypes: ContentType[]
}

const List: React.FunctionComponent<ListProps> = ({ contentTypes, match }) => {
  return (
    <>
      <h1>Content types</h1>
      <dl>
        {contentTypes.map(ct => 
          <React.Fragment key={ct.id}>
            <dt><Link to={{ pathname: `${match.url}/edit/${ct.id}` }}>{ct.name}</Link></dt>
            <dd>{ct.description}</dd>
          </React.Fragment>
        )}
      </dl>
    </>
  );
};

export default List;