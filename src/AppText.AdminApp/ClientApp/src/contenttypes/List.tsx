import React from 'react';
import { ContentType } from './models';

interface ListProps {
  contentTypes: ContentType[]
}

const List: React.FunctionComponent<ListProps> = ({ contentTypes }) => {
  return (
    <>
      <h1>Content types</h1>
      <dl>
        {contentTypes.map(ct => 
          <React.Fragment key={ct.id}>
            <dt>{ct.name}</dt>
            <dd>{ct.description}</dd>
          </React.Fragment>
        )}
      </dl>
    </>
  );
};

export default List;