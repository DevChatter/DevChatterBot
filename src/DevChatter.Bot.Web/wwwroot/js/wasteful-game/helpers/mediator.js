export class Event {
  /**
   * @public
   * @param {object} args event args
   */
  constructor(args) {
    this._process = true;
    this._args = args || {};
  }

  /**
   * @public
   * @returns {boolean} process
   */
  get process() {
    return this._process;
  }

  /**
   * @public
   * @param {boolean} process process
   */
  set process(process) {
    this._process = process;
  }

  /**
   * @public
   * @returns {object} args
   */
  get args() {
    return this._args;
  }

  /**
   * @public
   * @param {object} args eventargs
   */
  set args(args) {
    this._args = args;
  }
}

class Mediator {
  constructor() {
    this.channels = {};
  }

  /**
   * @public
   * @param {symbol|string} channel channel to subscrube to
   * @param {function} fn callback function
   */
  subscribe(channel, fn) {
    if (!this.channels[channel]) {
      this.channels[channel] = [];
    }

    this.channels[channel].push({
      context: this,
      callback: fn
    });
  }

  /**
   * @public
   * @param {symbol|string} channel channel to unsubscribe from
   * @param {function} fn callback function to remove
   */
  unsubscribe(channel, fn) {
    if (!this.channels[channel]) {
      return;
    }

    const index = this.channels[channel].indexOf(fn);
    if(index !== -1) {
      this.channels[channel].splice(index, 1);
    }
  }

  /**
   * @public
   * @param {symbol|string} channel channel to publish to
   * @param {Event} args event args
   * @returns {Event} event
   */
  publish(channel, args) {
    let retVal = new Event(args);
    if (!this.channels[channel]) {
      return retVal;
    }

    this.channels[channel].forEach(function(subscription) {
      Object.assign(retVal, subscription.callback.apply(subscription.context, [retVal]));
      if(!retVal.process) {
        return retVal;
      }
    });

    return retVal;
  }

  /**
   * @public
   * @param {object} obj object to install onto
   */
  installTo(obj) {
    obj.channels = {};
    obj.publish = this.publish;
    obj.subscribe = this.subscribe;
    obj.unsubscribe = this.unsubscribe;
  }
}

const mediator = new Mediator();
export default mediator;
