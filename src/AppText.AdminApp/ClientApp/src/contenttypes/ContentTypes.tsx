import React from 'react';
import ProtectedRoute from '../auth/ProtectedRoute';
import { RouteComponentProps } from 'react-router';
import List from './List';
import Create from './Create';
import Edit from './Edit';
import { ContentType } from './models';
import { getContentTypes } from './api';
import AppContext from '../apps/AppContext'
import { App } from '../apps/models';

interface ContentTypesProps extends RouteComponentProps<{}> {
  currentApp?: App
}

interface ContentTypesState {
  contentTypes: ContentType[],
  isFetching: boolean,
}

class ContentTypes extends React.Component<ContentTypesProps, ContentTypesState>
{
  constructor(props) {
    super(props);

    this.state = {
      contentTypes: [],
      isFetching: false
    }    
  }

  loadData() {
    this.setState({ isFetching: true });

    getContentTypes(this.props.currentApp.id)
      .then(result => {
        this.setState({ contentTypes: result });
      })
      .catch(err => {
        console.log(err);
      })
      .then(() => this.setState({ isFetching: false }));    
  }

  componentDidMount(): void {
    if (this.props.currentApp) {
      this.loadData();
    }
  }

  render() {
    const { match } = this.props;
    const { contentTypes } = this.state;
    return (
      <>
        <ProtectedRoute path={match.url} component={List} contentTypes={contentTypes} />
        <ProtectedRoute path={`${match.url}/create`} component={Create} />
        <ProtectedRoute path={`${match.url}/edit/:id`} component={Edit} />
      </>
    );
  }
}

export default (props) => (
  <AppContext.Consumer>
    {appContext => <ContentTypes {...props} currentApp={appContext.currentApp} />}
  </AppContext.Consumer>
);