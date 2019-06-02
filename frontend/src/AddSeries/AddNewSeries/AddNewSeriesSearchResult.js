import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { icons, kinds, sizes } from 'Helpers/Props';
import HeartRating from 'Components/HeartRating';
import Icon from 'Components/Icon';
import Label from 'Components/Label';
import Link from 'Components/Link/Link';
import SeriesPoster from 'Books/SeriesPoster';
import AddNewSeriesModal from './AddNewSeriesModal';
import styles from './AddNewSeriesSearchResult.css';

class AddNewSeriesSearchResult extends Component {

  //
  // Lifecycle

  constructor(props, context) {
    super(props, context);

    this.state = {
      isNewAddSeriesModalOpen: false
    };
  }

  componentDidUpdate(prevProps) {
    if (!prevProps.isExistingSeries && this.props.isExistingSeries) {
      this.onAddSerisModalClose();
    }
  }

  //
  // Listeners

  onPress = () => {
    this.setState({ isNewAddSeriesModalOpen: true });
  }

  onAddSerisModalClose = () => {
    this.setState({ isNewAddSeriesModalOpen: false });
  }

  //
  // Render

  render() {
    const {
      ISBN,
      title,
      titleSlug,
      publishDate,
      publisher,
      status,
      overview,
      statistics,
      ratings,
      images,
      isExistingSeries,
      isSmallScreen
    } = this.props;

    const {
      isNewAddSeriesModalOpen
    } = this.state;

    const linkProps = isExistingSeries ? { to: `/series/${titleSlug}` } : { onPress: this.onPress };
    let seasons = '1 Season';

    return (
      <div>
        <Link
          className={styles.searchResult}
          {...linkProps}
        >
          {
            isSmallScreen ?
              null :
              <SeriesPoster
                className={styles.poster}
                images={images}
                size={250}
                overflow={true}
              />
          }

          <div>
            <div className={styles.title}>
              {title}

              {
                !title.contains(publishDate) && publishDate ?
                  <span className={styles.year}>
                    ({new Date(publishDate).getFullYear()})
                  </span> :
                  null
              }

              {
                isExistingSeries ?
                  <Icon
                    className={styles.alreadyExistsIcon}
                    name={icons.CHECK_CIRCLE}
                    size={36}
                    title="Already in your library"
                  /> :
                  null
              }
            </div>

            <div>
              <Label size={sizes.LARGE}>
                <HeartRating
                  rating={ratings.value}
                  iconSize={13}
                />
              </Label>

              {
                publisher ?
                  <Label size={sizes.LARGE}>
                    {publisher}
                  </Label> :
                  null
              }

              {
                status === 'ended' ?
                  <Label
                    kind={kinds.DANGER}
                    size={sizes.LARGE}
                  >
                  Ended
                  </Label> :
                  null
              }
            </div>

            <div className={styles.overview}>
              {overview}
            </div>
          </div>
        </Link>

        <AddNewSeriesModal
          isOpen={isNewAddSeriesModalOpen && !isExistingSeries}
          isbn={ISBN}
          publishDate={publishDate}
          title={title}
          overview={overview}
          images={images}
          onModalClose={this.onAddSerisModalClose}
        />
      </div>
    );
  }
}

AddNewSeriesSearchResult.propTypes = {
  tvdbId: PropTypes.number.isRequired,
  title: PropTypes.string.isRequired,
  titleSlug: PropTypes.string.isRequired,
  publishDate: PropTypes.string.isRequired,
  network: PropTypes.string,
  status: PropTypes.string.isRequired,
  overview: PropTypes.string,
//   statistics: PropTypes.object.isRequired,
  ratings: PropTypes.object.isRequired,
  images: PropTypes.arrayOf(PropTypes.object).isRequired,
  isExistingSeries: PropTypes.bool.isRequired,
  isSmallScreen: PropTypes.bool.isRequired
};

export default AddNewSeriesSearchResult;
